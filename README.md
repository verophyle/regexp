# Verophyle.Regexp

| ----- | ----- |
| Build | [![AppVeyor](https://img.shields.io/appveyor/ci/kulibali/regexp.svg)]() |
| NuGet | [![NuGet](https://img.shields.io/nuget/dt/Verophyle.Regexp.svg)]() |

Simple DFA-based regular expressions for .NET.

Special characters are:

- `()`: enclose disjunction groups.
- `|`: separates disjunctions.
- `[]`: enclose a set of characters to match.
  - Can start with `^` to negate the set.
  - Can contain ranges of single characters, e.g.: `[0-9]`.
  - Can contain character classes, e.g.: `\w`, `\p{Lu}`.
- `\`: escapes the next character.  Some escaped characters have special meanings:
  - `\s`: matches whitespace.
  - `\w`: matches alphabetic letters.
  - `\d`: matches numeric digits.
  - `\p{Lu}`: matches the given [Unicode general category](https://en.wikipedia.org/wiki/Unicode_character_property#General_Category).
  - `\x{a0}`: matches the given hexadecimal value.
  - `\u{a0b1}`: matches the given Unicode code point (in hexadecimal).
  - Any other character following a `\` matches itself.  You can use this to match any of the special characters.
- `+`: matches the preceding letter, category, or group one or more times.
- `*`: matches the preceding letter, category, or group zero or more times.
- `?`: matches the preceding letter, category, or group zero or one times.
