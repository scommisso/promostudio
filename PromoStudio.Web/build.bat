:: Prior to running this script, run: npm install less from PromoStudio.web

echo Minifying scripts...
call buildScripts.bat
echo
echo Script Minification Complete! 
echo

echo Compiling LESS...
call buildCss.bat
echo
echo LESS Compilation Complete!