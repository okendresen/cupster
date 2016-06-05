import toml
from collections import Counter, defaultdict


def pairwise(iterable):
    "s -> (s0,s1), (s2,s3), (s4, s5), ..."
    a = iter(iterable)
    return zip(a, a)


def tournament_factory(configFile, userData, enterResults):
    with open(configFile) as f:
        config = toml.loads(f.read())
    if config['type'] == 'uefa-euro':
        tr = EuroTournament(userData, enterResults)
    else:
        tr = Tournament(userData, enterResults)
    tr.set_config(config)
    return tr


class Tournament(object):

    def __init__(self, results=None, isResults=False):
        self.groupStage = []
        self.results = results
        self.stageResults = []
        self.stage_two_names = {8: 'round-of-16',
                                4: 'quarter-final', 2: 'semi-final'}
        self.isResults = isResults
        self.thirds = []

    def load(self, toml_config):
        self.config = toml.loads(toml_config)

    def save_results(self):
        return toml.dumps(self.results)

    def get_config(self):
        return self.config

    def set_config(self, config):
        self.config = config

    def get_name(self):
        return self.config['name']

    def get_number_of_groups(self):
        return len(self.config['groups'])

    def get_info(self):
        lines = []
        lines.append('Tournament: {}'.format(self.get_name()))
        lines.append('Number of groups: {}'.format(
            self.get_number_of_groups()))
        return '\n'.join(lines)

    def get_groups_as_text(self):
        lines = []
        groupNum = ord('A')
        for group in self.config['groups']:
            lines.append('Group {}'.format(chr(groupNum)))
            groupNum += 1
            for team in group:
                lines.append('  {}'.format(team))
        return '\n'.join(lines)

    def setup_group_stage(self):
        groupNum = ord('A')
        for group in self.config['groups']:
            matches = []
            matches.append([group[0], group[1]])
            matches.append([group[2], group[3]])
            matches.append([group[0], group[2]])
            matches.append([group[3], group[1]])
            matches.append([group[3], group[0]])
            matches.append([group[1], group[2]])
            groupId = chr(groupNum)
            groupNum += 1
            self.groupStage.append((groupId, matches))

    def get_group_stage(self):
        lines = []
        self.setup_group_stage()
        gn = 0
        for id, matches in self.groupStage:
            lines.append('\nGroup {} stage one matches'.format(id))
            mn = 0
            for match in matches:
                if self.results.loaded:
                    result = self.results.get_stage_results(gn, mn)
                else:
                    result = ''
                lines.append('  {0} vs {1} . Selected {2}'.format(
                    match[0], match[1], result))
                mn += 1
            gn += 1
        return '\n'.join(lines)

    def user_group_stage_one_eval(self):
        self.setup_group_stage()
        gn = 0
        for id, matches in self.groupStage:
            print('\nGroup {} matches. Enter H, U or B'.format(id))
            points = []
            results = []
            mn = 0
            complete = True
            for match in matches:
                if self.results.loaded:
                    default = self.results.get_stage_results(gn, mn)
                else:
                    default = ''
                result = self.get_user_input(self.format_match_text(
                    match[0], match[1], default), default)
                if result == '-':
                    complete = False
                results.append(result)
                points.extend(self.match_score(match, result))
                mn += 1
            winner, runnerUp = self.tally_group_results(points)
            if complete:
                realWinner = self.resolve_winner(winner)
            else:
                realWinner = '-'
            print('Group winner: {}'.format(realWinner))
            if complete:
                realRunnerUp = self.resolve_runner_up(realWinner, runnerUp)
            else:
                realRunnerUp = '-'
            print('Runner-up: {}'.format(realRunnerUp))
            self.thirds.extend(self.get_non_qualifiers(points, realWinner, realRunnerUp))
            self.results.append_results(results)
            self.results.append_winners([realWinner, realRunnerUp])
            self.results.save()
            gn += 1

    def get_user_input(self, prompt, default):
        while True:
            result = input(prompt)
            if (not result and not default) or (result.lower() not in 'hub-'):
                print('Please enter one of H U B')
            else:
                break
        return result if result else default

    def match_score(self, match, result):
        win = []
        if result.lower() == 'h':
            for i in range(0, 3):
                win.append(match[0])
        elif result.lower() == 'b':
            for i in range(0, 3):
                win.append(match[1])
        else:
            win.append(match[0])
            win.append(match[1])
        return win

    def tally_group_results(self, score):
        top = Counter(score).most_common(3)
        self.stageResults.append(top)

        # Winner
        if top[0][1] > top[1][1]:
            winner = top[0][0]
        else:
            winner = (top[0][0], top[1][0])

        # Runner-up
        if isinstance(winner, tuple):
            runnerUp = winner
        elif top[1][1] > top[2][1]:
            runnerUp = top[1][0]
        else:
            runnerUp = (top[1][0], top[2][0])

        return winner, runnerUp

    def get_non_qualifiers(self, points, winner, runnerUp):
        rem = [t for t in points if t != winner]
        rem = [t for t in rem if t != runnerUp]
        return rem

    def select_team(self, teams):
        cnt = 1
        for team in teams:
            print('  {0}. {1}'.format(cnt, team))
            cnt += 1
        while True:
            try:
                ch = int(input('Select a number: '))
                if ch in [1, 2]:
                    break
            except ValueError:
                # not an integer
                if self.isResults:
                    ch = 0
                    break
                else:
                    continue

        if ch in [1, 2]:
            return teams[ch - 1]
        else:
            return '-'

    def resolve_winner(self, winner):
        res = ''
        if isinstance(winner, tuple):
            print('There is a draw. Please select the winner')
            res = self.select_team(winner)
        else:
            res = winner
        return res

    def resolve_runner_up(self, winner, runnerUp):
        res = ''
        if isinstance(runnerUp, tuple):
            if winner in runnerUp:
                r = list(runnerUp)
                r.remove(winner)
                res = r[0]
            else:
                print('There is a draw. Please select the runner-up')
                res = self.select_team(runnerUp)
        else:
            res = runnerUp
        return res

    def format_match_text(self, match1, match2, default):
        mt = '  {0} vs {1} '.format(match1, match2)
        l = len(mt)
        while l < 32:
            mt += '\t'
            l += 8
        mt += '[{}] '.format(default)
        return mt

    def user_stage_two_eval(self):
        qualifiers = self.results.get_winners()
        qualifiers = self.evalute_matches(
            qualifiers, len(qualifiers), self.get_stage_two_matches)
        while int(len(qualifiers) / 2) > 1:
            qualifiers = self.evalute_matches(qualifiers, int(
                len(qualifiers) / 2), self.get_final_matches)

    def evalute_matches(self, qualifiers, num, generate_matches_func):
        roundName = self.stage_two_names[num]
        print('\nSelect winners of', roundName)
        matches = generate_matches_func(qualifiers)
        qualifiers = []
        for match in matches:
            qualifiers.append(self.select_team(match))
        self.results.append_stage_two_winners(roundName, qualifiers)
        self.results.save()
        return qualifiers

    def get_stage_two_matches(self, qualifiers):
        matches = []
        for pair1, pair2 in pairwise(qualifiers):
            matches.append([pair1[0], pair2[1]])
            matches.append([pair2[0], pair1[1]])
        return matches

    def get_final_matches(self, qualifiers):
        matches = []
        for i in range(0, int(len(qualifiers)), 4):
            matches.append([qualifiers[i], qualifiers[i + 2]])
            matches.append([qualifiers[i + 1], qualifiers[i + 3]])
        return matches

    def user_final_eval(self):
        print('\nSelect winner of bronse final')
        bfinal = self.results.get_bronse_finalists()
        self.results.set_bronse_final_winner(self.select_team(bfinal))

        print('\nSelect winner of final')
        final = self.results.get_finalists()
        self.results.set_final_winner(self.select_team(final))
        self.results.save()


class EuroTournament(Tournament):
    """Specialization for UEFA Euro cup.
        Match setup from https://en.wikipedia.org/wiki/UEFA_Euro_2016#Knockout_phase"""
    def __init__(self, results=None, isResults=False):
        super().__init__(results, isResults)

    def get_top_four(self, points):
        top = Counter(points).most_common(4)
        res = [t[0] for t in top]
        return res

    def user_group_stage_one_eval(self):
        super().user_group_stage_one_eval()
        top4 = self.get_top_four(self.thirds)
        self.results.add_third_places(top4)
        self.results.save()

    def get_stage_two_matches(self, qualifiers):
        thirds = self.results.get_third_places()
        groups = []
        lookup = {}
        for team in thirds:
            group = self.find_group(team)
            groups.append(group)
            lookup[group] = team
        matchUp = self.get_match_ups(groups)

        matches = []
        # Match 1: Runner-up Group A v Runner-up Group C
        matches.append([qualifiers[0][1], qualifiers[2][1]])
        # Match 2: Winner Group D v 3rd Place Group B/E/F
        matches.append([qualifiers[3][0], lookup[matchUp[3]]])
        # Match 3: Winner Group B v 3rd Place Group A/C/D
        matches.append([qualifiers[1][0], lookup[matchUp[1]]])
        # Match 4: Winner Group F v Runner-up Group E
        matches.append([qualifiers[5][0], qualifiers[4][1]])
        # Match 5: Winner Group C v 3rd Place Group A/B/F
        matches.append([qualifiers[2][0], lookup[matchUp[2]]])
        # Match 6: Winner Group E v Runner-up Group D
        matches.append([qualifiers[4][0], qualifiers[3][1]])
        # Match 7: Winner Group A v 3rd Place Group C/D/E
        matches.append([qualifiers[0][0], lookup[matchUp[0]]])
        # Match 8: Runner-up Group B v Runner-up Group F
        matches.append([qualifiers[1][1], qualifiers[5][1]])

        return matches

    def find_group(self, team):
        gid = next((i for i, group in enumerate(self.config['groups']) if team in group), None)
        gid = chr(ord('A') + gid) if gid is not None else None
        return gid

    def get_match_ups(self, groups):
        combinations = defaultdict(list)
        combinations['ABCD'] = ['C', 'D', 'A', 'B']
        combinations['ABCE'] = ['C', 'A', 'B', 'E']
        combinations['ABCF'] = ['C', 'A', 'B', 'F']
        combinations['ABDE'] = ['D', 'A', 'B', 'E']
        combinations['ABDF'] = ['D', 'A', 'B', 'F']
        combinations['ABEF'] = ['E', 'A', 'B', 'F']
        combinations['ACDE'] = ['C', 'D', 'A', 'E']
        combinations['ACDF'] = ['C', 'D', 'A', 'F']
        combinations['ACEF'] = ['C', 'A', 'F', 'E']
        combinations['ADEF'] = ['D', 'A', 'F', 'E']
        combinations['BCDE'] = ['C', 'D', 'B', 'E']
        combinations['BCDF'] = ['C', 'D', 'B', 'F']
        combinations['BCEF'] = ['E', 'C', 'B', 'F']
        combinations['BDEF'] = ['E', 'D', 'B', 'F']
        combinations['CDEF'] = ['C', 'D', 'F', 'E']

        return combinations[''.join(sorted(groups))]

    def user_final_eval(self):
        print('\nSelect winner of final')
        final = self.results.get_finalists()
        self.results.set_final_winner(self.select_team(final))
        self.results.save()
