@echo off
echo Post run script, which is called after translation completed.
echo List of generated files is passed as arguments.

for %%I IN (%*) DO ECHO %%I