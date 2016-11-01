- title : F# for C# devs
- description : Introduction to F#
- author : Roman Provazník
- theme : night
- transition : none

***************************************************************************************************

# Functional .NET

## Join the **F#**orce

<br/><br/><br/><br/><br/>
### Roman Provazník

[@dzoukr](https://twitter.com/dzoukr) | [@fsharping](https://twitter.com/fsharping) | [fsharping.com](https://fsharping.com)

***************************************************************************************************

## F#

<table><tr><td class="table-leftcol">

**Multi**-paradigm

**Strongly** typed

**.NET** ** language

</td><td class="table-rightcol">
<img src="images/fsharp-logo.png" />
</td></tr></table>

***************************************************************************************************

## F#

<table><tr><td class="table-leftcol">

Version 1.0 in **May 2005**

Designed by **Don Syme**

Microsoft Research

</td><td class="table-rightcol">
<img src="images/donsyme.jpg" style="width:400px" />
</td></tr></table>

***************************************************************************************************

## Why join the **F#**orce?

***************************************************************************************************

## It is **.NET** language

-----------------------------------------

## Interoperability

<img src="images/highfive.jpg"/>

You can **call C# code from F#** and vice versa

-----------------------------------------

## Use your favorite editor

<img src="images/vs.png" style="width:150px; margin-right:10px;"/>

<img src="images/vscode.png" style="width:150px; margin-right:10px;"/>

<img src="images/atom.png" style="width:150px; margin-right:10px;"/>

<img src="images/xamarin.png" style="width:150px;"/>

Visual Studio, VS Code, Atom, Xamarin Studio

-----------------------------------------

## You know the platform

<img src="images/highfive2.jpg" />

Use your favorite Nuget packages

Known issues/fixes

***************************************************************************************************

## F# is **multiparadigm**

-----------------------------------------

## OOP!

<img src="images/cat.png" style="width:450px;"/>

-----------------------------------------

## FP!

<img src="images/dog.png" style="width:450px;"/>

-----------------------------------------

## Why not both?

<img src="images/catdog.jpg" style="width:450px;"/>

-----------------------------------------

## Combine OOP and FP

F# **takes the best** from both paradigms

Use **interfaces, abstract classes, inheritance**...

...as well as **high-order functions, lambdas, currying** and so on.

***************************************************************************************************

## **Lightweight** syntax

-----------------------------------------

## Lightweight syntax

<table><tr><td class="table-leftcol">

Based on **OCAML**

(Almost) **no** brackets

**No** semicolons

</td><td class="table-rightcol">
<img src="images/ocaml.png" style="width:300px;" />
</td></tr></table>

-----------------------------------------

## Lightweight syntax

```

// so powerful function!
let add x y = x + y

// ok, let`s make some heavy math here
add 22 20 // return 42
```


***************************************************************************************************

## **Type** inference

-----------------------------------------

## No types specified!


```
// look ma', no types!
let multiply x y = x * y

multiply 2 21 // returns 42
```
<br/>

**Type inference** works for you

Still **strongly-typed**

Increases development **speed**

***************************************************************************************************

## **REPL** included

-----------------------------------------

Read -> Evaluate -> Print -> Loop

It is **much easier** to write

1. Highlight your code
2. Press `Alt+Enter`
3. See the result
4. Maybe... convert it to unit test?

***************************************************************************************************

## Embedded **immutability**

-----------------------------------------

## Immutable data structures

Function types

Records

Discriminated Unions

Tuples

Lists

Option types

The Unit type

-----------------------------------------

## Why immutability?

Easy to **test**

Easy to **parallelize**

Immutability makes you code **predictable**

-----------------------------------------

## Why immutability?

<img src="images/immutable.jpg"/>

You **cannot screw** what you **cannot change**

***************************************************************************************************

## What we know so far?

### F# is **multiparadigm**<br/>**.NET** language, <br/>with **lightweight** syntax, <br/>**type** inference, <br/>and **immutable** data structures.

***************************************************************************************************

## But there is **much more**!

***************************************************************************************************

## Time for **deep dive**!

***************************************************************************************************

## Functions

-----------------------------------------

## Functions

<img src="images/fpvsoop.jpg" style="width:600px;"/>

**Essential** type for F#

**First-class** citizen

-----------------------------------------

## Functions

```
// simple function
let add x y = x + y
let result = add 10.0 20.0

// anonymous function
let lambdaAdd = fun a b -> a + b

// automatic generic
let printWhatever myParam = sprintf "String value is %A" myParam
```

<br/>

Still **readable**, right?

***************************************************************************************************

## Tuples

-----------------------------------------

## Tuples

Data structure with **N** elements

```
// simple tuple
let myTuple = "ahoj", 42

// destructuring
let name, value = myTuple

// ignore some values
let _, value = myTuple

// use built-in functions
let first = fst myTuple
let second = snd myTuple
```

Great for **temporary** values (no *Info classes needed)

Keep them **short**!

-----------------------------------------

## Tuples

Watch out for **comma** in functions!

```
// Two parameters
let normalAdd x y = x + y

// One!!! parameter (tuple)
let tupledAdd (x,y) = x + y
```

***************************************************************************************************

## Lists

-----------------------------------------

## Lists

F# list **is not** .NET List class

**Immutable** by default

**Easy** to work with

-----------------------------------------

## Lists

```
// basics
let numbers = [0;1;2;3;4;5]
let strings = ["a";"b";"c"]
let tuples = [("ahoj",1);("nazdar",2)]

let zeroToTen = [0..10]
let zeroToHundredByTen = [0..10..100]
let down = [100..-10..0]

List.length numbers // 6 elements
List.rev numbers // reverse them

```

-----------------------------------------

## Lists

```
// join them
let czGreetings = ["Cau";"Nazdar";"Te pero"]
let enGreetings = ["Hi"; "Hello"]
let joined = czGreetings @ enGreetings // join lists

let prepended = "Pocem tak jaka?" :: czGreetings // prepend to list
```

-----------------------------------------

## Lists


```
// list comprehension
let generatedEvens = [
        for i in 1..10 do 
            if i%2=0 then yield i
    ]

let differentGen = [ for i = 0 to 10 do yield i]
```

-----------------------------------------

## Lists


```
let result = 
    [0..10..100]
    |> List.filter (fun x -> x > 50)
    |> List.map ((*) 2)
    |> List.rev
    |> List.take 3
    |> List.skip 1

// result is : int list = [180; 160] 
```

See module **Microsoft.FSharp.Collections.List**

***************************************************************************************************

## Discriminated Unions

-----------------------------------------

## Discriminated Unions

Value that can be **one** of a number of **named cases**, possibly each with **different** values and types

-----------------------------------------

## Discriminated Unions

```
type Shape =
    | Rectangle of width : float * length : float
    | Circle of radius : float
    | Prism of width : float * float * height : float

type Size = Small | Medium | Large

type Result = 
  | Success                // no string needed for success state
  | ErrorMessage of string // error message needed 

```

-----------------------------------------

## Discriminated Unions

```
// tennis game designed on 6 rows
type Points = Zero | Fifteen | Thirty | Fourty
type Player = A | B
type Game =
    | Score of Points * Points
    | Advantage of Player
    | Victory of Player
```

Using Discriminated Unions makes<br/>**illegal states unrepresentable**


***************************************************************************************************

## Option

-----------------------------------------

## Do you remember your good old friend **NULL**?

<img src="images/null.gif" style="width:600px;"/>

-----------------------------------------

## F# has **no NULL**

also known as *billion dollar mistake*

-----------------------------------------

## Yeap.

-----------------------------------------

## NO

-----------------------------------------

## NULL!

-----------------------------------------

## No NULL!

<img src="images/indiandance.gif"/>

-----------------------------------------

## No NULL

Use **Option** type instead

```
let printInt (num:int option) =
   match num with
   | Some(n) -> printf "Number was %i" n
   | None    -> printf "Sorry, no number, go home"
```

-----------------------------------------

## No NULL

```
type Option<'a> =         // use a generic definition
   | Some of 'a           // valid value
   | None                 // missing

```

Maybe you noticed: Option is built-in **discriminated union**

***************************************************************************************************

## Records

-----------------------------------------

## Records

```
type Person = {
    Name : string
    Surname : string
    Age : int
}

let person = { Name = "Roman"; Surname = "Provaznik"; Age = 34 }
let age = person.Age
let name = person.Name
```

Great for storing **contextual** values

-----------------------------------------

## Records

```
let person = { Name = "Roman"; Surname = "Provaznik"; Age = 34 }
let person2 = { Name = "Roman"; Surname = "Provaznik"; Age = 34 }

let areEqual = person = person2 // true!
```

Structural **comparison** and **equality**

-----------------------------------------

## Records

```
let person = { Name = "Roman"; Surname = "Provaznik"; Age = 34 }

person.Name <- "Mirecek" // just nope!
```

**Immutable** by default

-----------------------------------------

## Records

```
let person = { Name = "Roman"; Surname = "Provaznik"; Age = 34 }

let mirecek = { person with Name = "Mirecek"; Surname = "Pospisil" }
```

But easy to **clone**

-----------------------------------------

## Records

```
type Role = 
    | CodeMonkey 
    | Manager of Employee 

and Employee = {
    Name : string
    Role : Role
    Age: int option
}

let monkey = { Name = "Roman"; Role = CodeMonkey; Age = Some(34)}
let manager = { Name = "Jaromir"; Role = Manager(monkey); Age = None}
```

Work **great** with other types (even functions!)

***************************************************************************************************

## Let`s dive even **deeper**!

***************************************************************************************************

<img src="images/deeper.jpg" style="width:600px;"/>

***************************************************************************************************

## Pattern Matching

-----------------------------------------

## Pattern Matching

Patterns are **rules** for transforming input data. 

They are used to **compare** data with a **logical** structure(s), **decompose** data into constituent **parts**, or **extract** information from data in various ways.

-----------------------------------------

## WAT?

<img src="images/wat.png" style="width:450px;"/>

-----------------------------------------

## Pattern Matching

It is something like **switch**...

...on heavy drugs.

-----------------------------------------

## Pattern Matching

```
let tupleOne = "Roman", 34
let tupleTwo = "Pablo", 44
let tupleThree = "Roman", 36

let trollMe value = 
    match value with
    | "Roman", 34   -> printf "Is that you?"
    | "Roman", _    -> printf "Oh, different Roman"
    | _, 100        -> printf "You are old as F**K"
    | name, age     -> printf "You are %s and %i years old" name age
```

-----------------------------------------

## Pattern Matching

```
let trollMeSoftly value = 
    match value with
    | _, age when age > 100       -> printf "Hey man, you are walking dead!"
    | name, _ when name = "Ivo"   -> printf "Hi Ivo"
    | _                           -> printf "Gotcha all!"
```

Use **when** condition for advanced filtering

-----------------------------------------

## Pattern Matching

```
type Permissions = Read | Write
type Access = Anonymous | Authorized of Permissions
type User = {
    Login : string
    Rights: Access
}

let displayLogin user =
    match user with
    | { Login = "Jakub" }       -> "You are good to go, Kubo"
    | { Rights = Anonymous }    -> "Anonymous access"
    | { Login = l; Rights = Authorized(perm) } -> 
        match perm with
        | Read  -> sprintf "You are %s and you can read only" l
        | Write  -> sprintf "You are %s and you can write" l

```

Works great with all **F# types** (Records, Discriminated Unions, ...)

-----------------------------------------

## Pattern Matching

```
let divide x y =
    try
        Some(x / y)
    with
    | :? System.DivideByZeroException as ex -> 
        printfn "Catched division by zero %s" ex.Message
        None
    | ex when ex.Message.Contains("Needle") -> None // use when
    | _ -> 
        printfn "Catched different exception"
        None

```

Try-catch is also pattern matching

-----------------------------------------

## Pattern Matching

Keeps your code **clean**

**Improves visibility** of your data flow

**Forces** you to **think** about **all possible** cases

***************************************************************************************************

## Currying

-----------------------------------------

## Currying

Named after mathematician **Haskell Brooks Curry**

Transformation of function with **N parameters**<br/> into **chain** of **N functions** with **1 parameter**

-----------------------------------------