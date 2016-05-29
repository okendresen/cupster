import toml


class TournamentResults(object):

    def __init__(self, result_file, user):
        self.results = {'stage-one': {'results': [], 'winners': []}, 'stage-two': {}, 'finals': {}, 'info': {'user': user}}
        self.loaded_results = None
        self.loaded = False
        self.fileName = result_file

    def load(self):
        with open(self.fileName) as f:
            self.loaded_results = toml.loads(f.read())
        self.loaded = True

    def save(self):
        s = open(self.fileName, 'w')
        s.write(toml.dumps(self.results))
        s.close()

    def copy_results(self):
        self.results = self.loaded_results

    def get_stage_results(self, groupNum, matchNum):
        res = ''
        if len(self.loaded_results['stage-one']['results']) > groupNum:
            group = self.loaded_results['stage-one']['results'][groupNum]
            if len(group) > matchNum:
                res = group[matchNum]
        return res

    def append_results(self, results):
        self.results['stage-one']['results'].append(results)

    def append_winners(self, winners):
        self.results['stage-one']['winners'].append(winners)

    def append_stage_two_winners(self, finalName, winners):
        if 'stage-two' in self.results:
            self.results['stage-two'][finalName] = winners
        else:
            stage = {finalName: winners}
            self.results['stage-two'] = stage

    def get_winners(self):
        return self.results['stage-one']['winners']

    def get_bronse_finalists(self):
        bf = list(self.results['stage-two']['quarter-final'])
        for team in self.results['stage-two']['semi-final']:
            bf.remove(team)
        return bf

    def get_finalists(self):
        return self.results['stage-two']['semi-final']

    def set_bronse_final_winner(self, winner):
        bfinal = {'bronse-final': winner}
        self.results['finals'] = bfinal

    def set_final_winner(self, winner):
        self.results['finals']['final'] = winner
