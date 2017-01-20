# TypeScript Compiler IO Performance Issue Reproduction

## Problem

At present, the [TypeScript compiler](https://www.typescriptlang.org/) ([_v2.1.5_](https://github.com/Microsoft/TypeScript/releases/tag/v2.1.5) at time of writing) suffers heavily from performance issues when dealing with projects containing 1000+ files. The bottleneck in the process is the IO write which takes 80-90% of the total compile time. Using the [`--outFile` compiler flag](https://www.typescriptlang.org/docs/handbook/compiler-options.html) to output all the transpiled JavaScript into a single file removes this bottleneck but this process doesn't play nicely with some module bundlers such as [webpack](https://webpack.github.io/) and &ndash; by extension &ndash; projects that depend on them for their build.

## Case study

A webpack-dependent project I develop for currently takes ~20 seconds to compile with `tsc.exe`. This is a huge productivity hit for our team! Running the compiler with the `--diagnostics` flag produces the following console output:
```
Files:         1430
Lines:       136554
Nodes:       751493
Identifiers: 248858
Symbols:     380932
Types:       153718
I/O read:     0.09s
I/O write:    9.92s
Parse time:   1.17s
Bind time:    0.54s
Check time:   4.02s
Emit time:   13.04s
Total time:  18.77s
```
As you can see from the results, our total compile time is around 19 seconds, of which over half that time is taken-up by writing to files. A project as large and complex as ours is naturally also going to take longer to parse and the like but nearly 10 seconds for IO write is excessive... When output to a single JavaScript file is enabled, the time taken for IO is negligible:
```
...
I/O read:     0.09s
I/O write:    0.22s
Parse time:   1.16s
Bind time:    0.56s
Check time:   4.02s
Emit time:    3.06s
Total time:   8.80s
```
However, since we rely on webpack as part of our build process, outputting to a single file isn't an option for us.

## Reproduction

Since the aforementioned project is closed-source, I've written a small C# tool to generate a simple (yet large), tree-structured TypeScript project that can be used to demonstrate the problem. Using the TypeScript compiler to transpile the TS output of the tool to JavaScript with `tsc Main.ts --diagnostics` results in comparable build times to our proprietary project:
```
Files:         2552
Lines:        28620
Nodes:       134614
Identifiers:  37571
Symbols:      31717
Types:        12250
I/O read:     0.12s
I/O write:    5.67s
Parse time:   0.52s
Bind time:    0.09s
Check time:   0.32s
Emit time:    6.19s
Total time:   7.11s
```
Our project's build times are naturally going to be greater than this simple case as the source is much more complex but the effect is still clearly visible here, with over 5 seconds being spent on IO write, especially when compared to compiling with `tsc Main.ts --diagnostics --module amd --outFile Main.js` where IO write drops to under a tenth of a second:
```
...
I/O read:     0.14s
I/O write:    0.09s
Parse time:   0.48s
Bind time:    0.09s
Check time:   0.32s
Emit time:    0.45s
Total time:   1.34s
```
There's a constant called `modulesPerSubdirectory` in the C# source file that can be increased to further exacerbate the issue for testing and benchmarking purposes if necessary. It's currently set to 50 which is sufficient to demonstrate the performance issue.

Precompiled TypeScript output from the C# script with the default module-per-subdirectory value can be found in the `GeneratedTypeScript` directory of the repository:

https://github.com/BluewireTechnologies/slow-tsc-outfile-reproduction/tree/master/GeneratedTypeScript

## Licence

Copyright © Bluewire Technologies Ltd 2017
