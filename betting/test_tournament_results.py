import unittest
from hamcrest import *
from betting import TournamentResults


class TestTournamentResults(unittest.TestCase):

    def test_create(self):
        user = 'user'
        testfile = 'no file'
        result = TournamentResults(testfile, user)
        assert_that(result, instance_of(TournamentResults))

    def test_add_get_third_places(self):
        user = 'user'
        testfile = 'no file'
        result = TournamentResults(testfile, user)
        thirds = [['italy', 'A'], ['brasil', 'B'], ['england', 'C'], ['sweden', 'D']]
        result.add_third_places(thirds)
        actual = result.get_third_places()
        assert_that(actual, is_(equal_to(thirds)))
