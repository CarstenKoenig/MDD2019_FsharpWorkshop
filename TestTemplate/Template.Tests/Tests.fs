module ``test Greetings_sayHello``

open Xunit
open FsUnit.Xunit

open Template.Greetings
open Hedgehog

/// Einfacher Unit-Test
[<Fact>]
let ``should begin with Hello`` () =
    sayHello "Programmer" |> should startWith "Hello"

/// einfacher Property-Based Test
[<Fact>]
let ``should contain the given name`` () =
    property {
        let! name = Gen.string (Range.linear 5 15) Gen.latin1
        sayHello name |> should haveSubstring name
    }
    |> Property.check