#!/bin/bash
aspell -t --lang=es --encoding=ISO-8859-1 --add-tex-command="includefigure pPp" --add-tex-command="ctable oppp" --add-tex-command="textit p" --add-tex-command="acl p" --add-tex-command="ac p" --add-tex-command="acs p" --add-tex-command="texttt p" --personal=./.aspell.es.pws check "$1"
