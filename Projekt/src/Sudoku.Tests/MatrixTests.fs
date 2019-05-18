namespace Sudoku.Tests

open Sudoku
open Xunit
open Hedgehog

open Sudoku.Game

[<AutoOpenAttribute>]
module private Generators = 
    let zifferGen =
        Gen.int (Range.linear 1 9)

    let zelleGen =
        Gen.choice
            [
                Gen.constant Leer
                Gen.map Zahl zifferGen
            ]

    let eingabeGen : Gen<Eingabe> =
        let reiheGen = Gen.list (Range.singleton 9) zelleGen
        Gen.list (Range.singleton 9) reiheGen


module ``Matrix Involutionen für Sudoku Eingaben`` =
    open Matrix

    [<Fact>]
    let ``transponiere >> transponiere = id`` () =
        property {
            let! g = eingabeGen
            return transponiere (transponiere g) = g
        }
        |> Property.check


    [<Fact>]
    let ``transponiere für Beispiel ist korrekt`` () =
        transponiere [[1;2];[3;4]] = [[1;3];[2;4]]


    [<Fact>]
    let ``Zeilen >> Zeilen = id`` () =
        property {
            let! g = eingabeGen
            return zeilen (zeilen g) = g
        }
        |> Property.check


    [<Fact>]
    let ``zeilen für Beispiel ist korrekt`` () =
        zeilen [[1;2];[3;4]] = [[1;2];[3;4]]


    [<Fact>]
    let ``Spalten >> Spalten = id`` () =
        property {
            let! g = eingabeGen
            return spalten (spalten g) = g
        }
        |> Property.check


    [<Fact>]
    let ``spalten für Beispiel ist korrekt`` () =
        spalten [[1;2];[3;4]] = [[1;3];[2;4]]


    [<Fact>]
    let ``Blöcke >> Blöcke = id`` () =
        property {
            let! g = eingabeGen
            return bloecke 3 (bloecke 3 g) = g
        }
        |> Property.check


    [<Fact>]
    let ``bloecke für Beispiel ist korrekt`` () =
        bloecke 2 [[1;2;3;4];[5;6;7;8];[9;10;11;12];[13;14;15;16]] = 
            [[1;2;5;6];[3;4;7;8];[9;10;13;14];[11;12;15;16]]

