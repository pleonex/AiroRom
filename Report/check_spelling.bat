@echo off
chcp 28591
"C:\Program Files (x86)\Aspell\bin\aspell.exe" -t --lang=es --encoding=ISO8859-1 --add-tex-command="includefigure pPp" --add-tex-command="ctable oppp" --add-tex-command="textit p" --add-tex-command="acl p" --add-tex-command="ac p" --add-tex-command="acs p" --add-tex-command="texttt p" --personal="%~p0.aspell.es.pws" check "%1"
