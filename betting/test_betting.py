import unittest
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

    def test_match_winner(self):
        match = ['brasil', 'kroatia']
        self.init('')
        assert_that(self.cup.match_score(match, 'h'), is_(equal_to('brasil')))
        assert_that(self.cup.match_winner(match, 'b'), is_(equal_to('kroatia')))
        assert_that(self.cup.match_winner(match, 'u'), is_(equal_to('')))

if __name__ == '__main__':
    unittest.main()
