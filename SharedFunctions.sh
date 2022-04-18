##From https://unix.stackexchange.com/questions/8759/best-practice-to-use-in-bash ()
die() {
    IFS=' ' # make sure "$*" is joined with spaces

    # output the arguments if any on stderr:
    [ "$#" -eq 0 ] || printf '%s\n' "$*" 1>&2
    exit 1
}

##From https://dev.to/meleu/how-to-join-array-elements-in-a-bash-script-303a
joinByString() {
  local separator="$1"
  shift
  local first="$1"
  shift
  printf "%s" "$first" "${@/#/$separator}"
}