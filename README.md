# FSharpFsxSolution
An example of a project with tests using FSX files using these conventions

## build

`./build.sh` will build, run the tests, and output to `/build`

`./build.sh -h` will do a 'hard' build, reinstalling packages and recreating script references

The compiled library will take the name of the root folder

## src

`Main` is the entry point for your app

`References` contains all the `fsx` files in your app except `Main`.
This will need to be maintained by hand to ensure correct order. Every FSX file has to `#load` this.

## tests

Test are discovered by the script so just add them to this directory