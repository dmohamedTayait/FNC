REM Create the folders
md "C:\PDF Writer"
md "C:\PDF Writer\Application Data"
md "C:\PDF Writer\Common Application Data"
md "C:\PDF Writer\Common Application Data\PDF Writer"
md "C:\PDF Writer\Common Application Data\PDF Writer\PDF Writer - bioPDF"
md "C:\PDF Writer\Common Application Data\PDF Writer\Bullzip PDF Printer"
md "C:\PDF Writer\Local Application Data"
md "C:\PDF Writer\Output"

REM Install the global.ini
copy global.ini "C:\PDF Writer\Common Application Data\PDF Writer\PDF Writer - bioPDF\"
copy global.ini "C:\PDF Writer\Common Application Data\PDF Writer\Bullzip PDF Printer\"
REM Install the registry keys
regedit /s "shared printer.reg"