from cx_Freeze import setup, Executable

# Dependencies are automatically detected, but it might need
# fine tuning.
exebuildOptions = dict(packages = [], excludes = [], include_files=['vm2014.toml'])
msibuildOptions = dict(upgrade_code='abd4d1a8-3541-43a3-8c2e-5e6094ae60ad')

base = 'Console'

executables = [
    Executable('tipping.py', base=base)
]

setup(name='tipping',
      version = '1.0.2',
      description = 'Tipp resultater av VM',
      options = dict(build_exe = exebuildOptions, bdist_msi=msibuildOptions),
      executables = executables)
