#load "References.fsx"

[<EntryPoint>]
let main args =
  ArraySerializer.serialize args
  |> printfn "%s"
  0