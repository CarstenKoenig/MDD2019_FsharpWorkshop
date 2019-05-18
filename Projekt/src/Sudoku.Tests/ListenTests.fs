namespace Sudoku.Tests

open Sudoku
open Xunit
open Hedgehog


module ``group und ungroup`` =
    open Matrix

    [<Fact>]
    let ``alle Elemente von group 3 haben Länge 3`` () =
        property {
            let! xs = Gen.list (Range.linear 0 10) (Gen.int (Range.linear 1 50))
            // stelle sicher, dass xs genügend Elemente hat
            let xs = xs @ xs @ xs
            return 
                group 3 xs
                |> List.forall (fun l -> List.length l = 3)
        }
        |> Property.check


    [<Fact>]
    let ``group 3 >> ungroup = id (für gültige Eingaben)`` () =
        property {
            let! xs = Gen.list (Range.linear 0 10) (Gen.int (Range.linear 1 50))
            // stelle sicher, dass xs genügend Elemente hat
            let xs = xs @ xs @ xs
            return ungroup (group 3 xs) = xs
        }
        |> Property.check
        

module ``crossProd`` =

    [<Fact>]
    let ``crossProd einer Liste von singletons ist enthält nur diese``() =
        property {
            let! xs = Gen.list (Range.linear 0 10) (Gen.int (Range.linear 1 50))
            let xss = List.map Seq.singleton xs
            return List.ofSeq (crossProd xss) = [xs]
        }
        |> Property.check


    [<Fact>]
    let ``|crossProd xss| = Produkt der Längen der xs in xss``() =
        property {
            let! xs = Gen.list (Range.linear 0 10) (Gen.int (Range.linear 1 50))
            let! ys = Gen.list (Range.linear 0 10) (Gen.int (Range.linear 1 50))
            let xss = [xs; ys]
            return Seq.length (crossProd xss) = xs.Length * ys.Length
        }
        |> Property.check