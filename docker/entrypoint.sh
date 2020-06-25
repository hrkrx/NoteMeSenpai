mongod&
sleep 2
while :
do
git clone --recurse-submodules https://github.com/hrkrx/NoteMeSenpai
cd /NoteMeSenpai/NoteMeSenpai && dotnet publish -o /build/
cd /build
dotnet NoteMeSenpai.dll
done