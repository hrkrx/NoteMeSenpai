FROM mcr.microsoft.com/dotnet/core/sdk

RUN apt update
RUN apt upgrade
RUN apt install nano
RUN apt install git apt-utils systemd -y
RUN git clone --recurse-submodules https://github.com/hrkrx/NoteMeSenpai
RUN mkdir build
RUN cd NoteMeSenpai/NoteMeSenpai && dotnet publish -o /build/

COPY api.key build
COPY settings.json build

RUN apt install gnupg curl -y
RUN curl https://www.mongodb.org/static/pgp/server-4.2.asc | apt-key add -
RUN echo "deb http://repo.mongodb.org/apt/debian buster/mongodb-org/4.2 main" | tee /etc/apt/sources.list.d/mongodb-org-4.2.list
RUN apt update
RUN mkdir /data
RUN mkdir /data/db
RUN apt install mongodb-org -y
RUN cp /usr/bin/mongod /etc/init.d/

COPY entrypoint.sh /
RUN chmod +x entrypoint.sh

ENTRYPOINT [ "sh", "/entrypoint.sh" ]