#!/bin/bash
aspell -t --lang=es --encoding=ISO-8859-1 --add-tex-command="includefigure pPp" --add-tex-command="ctable oppp" --personal=./.aspell.es.pws check "$1"
