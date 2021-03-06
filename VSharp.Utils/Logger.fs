namespace VSharp

module Logger =
    open System

    let Error = 1
    let Warning = 2
    let Info = 3
    let Trace = 4

    let mutable current_log_level = Trace
    let mutable current_text_writer = Console.Out

    let LevelToString = function
        | 1 -> "Error"
        | 2 -> "Warning"
        | 3 -> "Info"
        | 4 -> "Trace"
        | _ -> "Unknown"

    let private writeLineString vLevel message =
        let res = sprintf "[%s] [%A] %s" (LevelToString vLevel) System.DateTime.Now message
        current_text_writer.WriteLine(res)
        current_text_writer.Flush()

    let public printLog vLevel format =
        Printf.ksprintf (fun message -> if current_log_level >= vLevel then writeLineString vLevel message) format

    let public printLogLazy vLevel format (s : Lazy<_>) =
        if current_log_level >= vLevel then
            Printf.ksprintf (writeLineString vLevel) format (s.Force())
