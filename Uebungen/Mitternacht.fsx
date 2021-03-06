#r "FsCheck.dll"

open System

let nahe0 (x : float) : bool =
    abs x < 0.0000001

type QuadGleichung = float * float * float
type Loesung       = float

type Loesungen =
  | Keine
  | Alles
  | EineVon of Loesung list

let loeseLinear (a : float, b : float) : Loesungen =
  if nahe0 a && nahe0 b then Alles
  elif nahe0 a then Keine
  else EineVon [-b / a]

// sollte ohne `if a = 0 ...` vorgeführt werden - (Quickcheck)
let loese ((a,b,c) : QuadGleichung) : Loesungen =
    if nahe0 a then loeseLinear (b,c) else
    let nenner = 2.0*a
    let diskriminante = b*b - 4.0*a*c
    match diskriminante with
      | d when nahe0 d -> EineVon [-b / nenner]
      | d when d < 0.0 -> Keine
      | d ->
        let wurzel = sqrt d
        EineVon [(-b-wurzel)/nenner; (-b+wurzel)/nenner]     

loese (2.0, -10.0, 12.0)



// ************* TESTS ******************

let pruefeLoesung ((a,b,c) : QuadGleichung) (x : float) : bool =
    a*x*x + b*x + c |> nahe0

pruefeLoesung (2.0, -10.0, 12.0) 3.0

let pruefeLoesungen (qg : QuadGleichung) (ls : Loesung list) : bool =
  List.forall (pruefeLoesung qg) ls

module ``Teste die Lösungsformel`` =
  open FsCheck

  type Marker = class end

  // feige: ignoriere Probleme mit float (max/min/inf)
  let ``in Polynom eingesetzte Loesungen ergeben 0`` (a : int, b : int, c : int) : bool =
    let qg = (float a, float b, float c)
    match loese qg with
    | Keine      -> true
    | Alles      -> pruefeLoesungen qg [-1.0;0.0;1.0;2.0]
    | EineVon ls -> pruefeLoesungen qg ls

  let checkAll() =
    Check.QuickAll (typeof<Marker>.DeclaringType)

  checkAll()
// ende  
