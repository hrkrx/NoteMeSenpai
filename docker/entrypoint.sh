mongod&
sleep 2
while :
do
cd /NoteMeSenpai/NoteMeSenpai && git pull && dotnet publish -o /build/
cd /build
dotnet NoteMeSenpai.dll
done