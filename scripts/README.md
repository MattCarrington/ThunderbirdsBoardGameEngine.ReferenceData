# Local Git Hooks

Run once after each clone, from the repository root:

```powershell
./scripts/install-hooks.ps1
```

The installer writes Git's local pre-commit hook. The wrapper resolves this
repository at invocation time and uses PowerShell 7 (pwsh) when available,
or Windows PowerShell otherwise. It does not stage files, change global Git
settings, or depend on the original application's path. Custom hook
configurations and unmanaged existing hooks are left unchanged. Running the
installer again refreshes a hook it previously installed.

Before a commit, the hook verifies formatting without rewriting source, builds
the standalone solution in Release, then runs every *.UnitTests.csproj under
tests/UnitTests without rebuilding. Any failed command blocks the commit.
An empty unit-test discovery result also fails. Component tests remain part
of the full CI suite; they are not included in this local unit-test gate.

To run the same checks manually:

```powershell
./scripts/pre-commit.ps1
```

Git does not install hooks from a clone automatically. The scripts are committed,
but each local clone needs the installer. GitHub CI remains an independent check.
