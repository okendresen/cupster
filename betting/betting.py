import argparse
import os
import getpass
import shutil
from tournament import *
from tournamentresults import *


def main():
    parser = argparse.ArgumentParser()
    parser.add_argument('tournament_file', help='a TOML file defining the tournament')
    parser.add_argument('-g', '--print_groups', help='print out groups in tournament',
                        action='store_true')
    parser.add_argument('-m', '--print_matches', help='print out stage one matches in tournament',
                        action='store_true')
    parser.add_argument('-r', '--results', help='enter results from tournament',
                        action='store_true')
    parser.add_argument('-s', '--skip_group_stage', help='Go directly to stage two finals',
                        action='store_true')
    args = parser.parse_args()

    # Create output name
    root, ext = os.path.splitext(args.tournament_file)
    if args.results:
        print("===== ENTERING RESULTS =====")
        user = "actual"
    else:
        user = getpass.getuser()
    outfile = '{0}-{1}.toml'.format(root, user)
    bakfile = '{0}-{1}.toml.bak'.format(root, user)

    # Backup results file it it exists
    if args.results and os.path.isfile(outfile):
        shutil.copyfile(outfile, bakfile)

    # Load results if they exists
    results = TournamentResults(outfile, user)
    if os.path.isfile(outfile):
        results.load()

    # Load tournament config
    tr = tournament_factory(args.tournament_file, results, args.results)

    print(tr.get_info())
    if args.print_groups:
        print(tr.get_groups_as_text())
    elif args.print_matches:
        print(tr.get_group_stage())
    elif args.skip_group_stage:
        results.copy_results()
        tr.user_stage_two_eval()
        tr.user_final_eval()
    else:
        tr.user_group_stage_one_eval()
        tr.user_stage_two_eval()
        tr.user_final_eval()


if __name__ == '__main__':
    main()
