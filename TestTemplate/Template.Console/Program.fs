// Learn more about F# at http://fsharp.org

open System
open Template.Greetings

[<EntryPoint>]
let main argv =
    sayHello "Programmer"
    |> Console.WriteLine
    0 // return an integer exit code
