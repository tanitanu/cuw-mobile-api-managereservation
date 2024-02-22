#!/bin/bash

################################################################################
#
# Loads the content of the YML file passed in as a program argument to a set
# of environmental variables matching the key/value setting pairs in the file.
#
# Usage: ./yaml_file_to_env_vars.sh yaml_file_path
#
################################################################################

FILE_PATH="$1"

################################################################################
#
# Parses YML files and makes their settings available as variables.
#
# See https://stackoverflow.com/questions/5014632/how-can-i-parse-a-yaml-file-from-a-linux-shell-script.
#
# Usage: eval $(parse_yaml my_file.yml)
#
# Accepts an argument that adds a prefix to the variables representing the
# YML file settings:
#
# Usage: eval $(parse_yaml my_file.yml "CONF_")
#
################################################################################
function ParseYaml {
   local prefix=$2
   local s='[[:space:]]*' w='[a-zA-Z0-9_]*' fs=$(echo @|tr @ '\034')
   sed -ne "s|^\($s\):|\1|" \
        -e "s|^\($s\)\($w\)$s:$s[\"']\(.*\)[\"']$s\$|\1$fs\2$fs\3|p" \
        -e "s|^\($s\)\($w\)$s:$s\(.*\)$s\$|\1$fs\2$fs\3|p"  $1 |
   awk -F$fs '{
      indent = length($1)/2;
      vname[indent] = $2;
      for (i in vname) {if (i > indent) {delete vname[i]}}
      if (length($3) > 0) {
         vn=""; for (i=0; i<indent; i++) {vn=(vn)(vname[i])("_")}
         printf("%s%s%s=\"%s\"\n", "'$prefix'",vn, $2, $3);
      }
   }'
}

if [ ! -f ${FILE_PATH} ]; then
  echo "File ${FILE_PATH} not found!"
  exit 1
fi

eval $(ParseYaml ${FILE_PATH})