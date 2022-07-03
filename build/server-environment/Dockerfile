FROM debian:stable AS BASE

WORKDIR /node
RUN apt update\
    && apt install -y curl apt-transport-https build-essential gnupg wget
RUN curl -sS https://dl.yarnpkg.com/debian/pubkey.gpg |  apt-key add -
RUN echo "deb https://dl.yarnpkg.com/debian/ stable main" | tee /etc/apt/sources.list.d/yarn.list

RUN curl -sL https://deb.nodesource.com/setup_14.x | bash -

RUN apt update\
    && apt install -y nodejs yarn

RUN node --version
RUN yarn --version

WORKDIR /aspnet
RUN wget https://packages.microsoft.com/config/debian/11/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
RUN dpkg -i packages-microsoft-prod.deb
RUN rm packages-microsoft-prod.deb

RUN apt update && \
    apt install -y dotnet-sdk-6.0

