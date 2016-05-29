import unittest
from hamcrest import *
from betting import TournamentResults


class TestTournamentResults(unittest.TestCase):

    def test_create(self):
        user = 'user'
        testfile = 'no file'
        result = TournamentResults(testfile, user)
        assert_that(result, instance_of(TournamentResults))
