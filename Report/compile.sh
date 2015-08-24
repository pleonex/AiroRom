#!/bin/bash
# Install texlive-scheme-tetex AND biber AND latexmk AND mscgen

# Generate diagrams
mscgen -T eps -i diagrams/nds_dwc.msc

# Generate document
latexmk -pdf Document.tex
