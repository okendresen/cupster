import unittest
from unittest.mock import Mock
from hamcrest import *
from tournament import *


class TestTournament(unittest.TestCase):

    def init(self, toml):
        self.cup = Tournament()
        self.cup.load(toml)

    def test_create_empty_Tournament(self):
        tr = Tournament()
        assert_that(tr, instance_of(Tournament))

    def test_load_returns_dict(self):
        self.init('')
        actual = self.cup.get_config()
        assert_that(actual, instance_of(dict))

    def test_load_returns_config(self):
        toml = 'name = "foo"\ngroups = [ ["op", "oop"], ["opo", "ioi"] ]'
        self.init(toml)
        config = self.cup.get_config()
        assert_that(config['name'], equal_to('foo'))

    def test_get_name(self):
        toml = 'name = "VM 2014"'
        self.init(toml)
        assert_that(self.cup.get_name(), is_(equal_to('VM 2014')))

    def test_get_number_of_groups(self):
        toml = 'name = "foo"\ngroups = [ ["op", "oop"], ["opo", "ioi"] ]'
        self.init(toml)
        assert_that(self.cup.get_number_of_groups(), is_(equal_to(2)))

    def test_match_score(self):
        match = ['brasil', 'kroatia']
        self.init('')
        assert_that(self.cup.match_score(match, 'h'), is_(equal_to(['brasil', 'brasil', 'brasil'])))
        assert_that(self.cup.match_score(match, 'b'), is_(equal_to(['kroatia', 'kroatia', 'kroatia'])))
        assert_that(self.cup.match_score(match, 'u'), is_(equal_to(['brasil', 'kroatia'])))

    def test_tally_group_results(self):
        self.init('')
        winners = ['brasil', 'brasil', 'brasil', 'brasil', 'england', 'england',
                   'england', 'sweden', 'sweden', 'cuba']
        first, second = self.cup.tally_group_results(winners)
        assert_that(first, is_(equal_to('brasil')))
        assert_that(second, is_(equal_to('england')))

    def test_tally_group_results_when_draw(self):
        self.init('')
        winners = ['brasil', 'brasil', 'brasil', 'england', 'england',
                   'england', 'sweden', 'sweden', 'cuba']
        first, second = self.cup.tally_group_results(winners)
        assert_that(first, is_(tuple), 'first')
        assert_that(second, is_(tuple), 'second')

        winners = ['brasil', 'brasil', 'brasil', 'brasil', 'england', 'england',
                   'sweden', 'sweden', 'cuba']
        first, second = self.cup.tally_group_results(winners)
        assert_that(first, is_(equal_to('brasil')))
        assert_that(second, is_(tuple))

    def test_get_non_qualifiers(self):
        self.init('')
        points = ['brasil', 'brasil', 'brasil', 'england', 'england',
                  'england', 'sweden', 'sweden', 'cuba']
        winner = 'brasil'
        runnerUp = 'england'
        remaining = ['sweden', 'sweden', 'cuba']
        actual = self.cup.get_non_qualifiers(points, winner, runnerUp)
        assert_that(actual, is_(equal_to(remaining)))


class TestEuroTournament(unittest.TestCase):

    def init(self, toml):
        self.cup = EuroTournament()
        self.cup.load(toml)

    def test_get_top_four(self):
        self.init('')
        points = ['brasil', 'brasil', 'brasil', 'england', 'england',
                  'england', 'sweden', 'sweden', 'cuba', 'lithuania',
                  'italy', 'italy', 'italy', 'italy']
        actual = self.cup.get_top_four(points)
        assert_that(actual, has_items('italy', 'brasil', 'england', 'sweden'))

    def test_find_group_name(self):
        toml = 'name = "foo"\ngroups = [ ["op", "oop"], ["opo", "ioi"] ]'
        self.init(toml)
        assert_that(self.cup.find_group('oop'), is_('A'))
        assert_that(self.cup.find_group('opo'), is_('B'))
        assert_that(self.cup.find_group('op'), is_('A'))
        assert_that(self.cup.find_group('not_there'), is_(None))

    def test_get_match_ups(self):
        self.init('')
        assert_that(self.cup.get_match_ups(['A', 'B', 'C', 'D']),
                    is_(equal_to(['C', 'D', 'A', 'B'])))
        assert_that(self.cup.get_match_ups(['A', 'B', 'C', 'E']),
                    is_(equal_to(['C', 'A', 'B', 'E'])))
        assert_that(self.cup.get_match_ups(['A', 'B', 'C', 'F']),
                    is_(equal_to(['C', 'A', 'B', 'F'])))
        assert_that(self.cup.get_match_ups(['A', 'B', 'D', 'E']),
                    is_(equal_to(['D', 'A', 'B', 'E'])))
        assert_that(self.cup.get_match_ups(['A', 'B', 'D', 'F']),
                    is_(equal_to(['D', 'A', 'B', 'F'])))
        assert_that(self.cup.get_match_ups(['A', 'B', 'E', 'F']),
                    is_(equal_to(['E', 'A', 'B', 'F'])))
        assert_that(self.cup.get_match_ups(['F', 'B', 'E', 'A']),
                    is_(equal_to(['E', 'A', 'B', 'F'])))

    def test_get_stage_two_matches(self):
        self.init('')
        qualifiers = [
            ['Winner A', 'Runner-up A'],
            ['Winner B', 'Runner-up B'],
            ['Winner C', 'Runner-up C'],
            ['Winner D', 'Runner-up D'],
            ['Winner E', 'Runner-up E'],
            ['Winner F', 'Runner-up F']
        ]
        self.cup.results = Mock()
        self.cup.results.get_third_places = Mock()
        self.cup.find_group = Mock()

        # Thirds from A, B, C, D
        self.cup.find_group.side_effect = ['A', 'B', 'C', 'D']
        thirds = ['3A', '3B', '3C', '3D']
        self.cup.results.get_third_places.return_value = thirds
        expected = [
            ['Runner-up A', 'Runner-up C'], ['Winner D', '3B'],
            ['Winner B', '3D'], ['Winner F', 'Runner-up E'],
            ['Winner C', '3A'], ['Winner E', 'Runner-up D'],
            ['Winner A', '3C'], ['Runner-up B', 'Runner-up F']
        ]
        actual = self.cup.get_stage_two_matches(qualifiers)
        assert_that(actual, is_(equal_to(expected)))

        # Thirds from B, C, E, F
        self.cup.find_group.side_effect = ['F', 'B', 'E', 'C']
        thirds = ['3F', '3B', '3E', '3C']
        self.cup.results.get_third_places.return_value = thirds
        expected = [
            ['Runner-up A', 'Runner-up C'], ['Winner D', '3F'],
            ['Winner B', '3C'], ['Winner F', 'Runner-up E'],
            ['Winner C', '3B'], ['Winner E', 'Runner-up D'],
            ['Winner A', '3E'], ['Runner-up B', 'Runner-up F']
        ]
        actual = self.cup.get_stage_two_matches(qualifiers)
        assert_that(actual, is_(equal_to(expected)))

if __name__ == '__main__':
    unittest.main()
