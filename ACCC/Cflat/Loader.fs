module Loader

open System.IO

let load (path: string) = 
    File.ReadAllText(path)