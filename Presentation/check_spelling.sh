#!/bin/bash
aspell -t --lang=es --encoding=utf-8 --add-tex-command="includefigure Pp" --personal=./.aspell.es.pws check "$1"
