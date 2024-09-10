FROM node:20-alpine3.18

ARG BASE_URL
ENV BASE_URL=${BASE_URL}

WORKDIR /frontend_app

RUN apk --no-cache add openssh g++ make python3 git

COPY package.json /frontend_app/
COPY package-lock.json /frontend_app/

RUN npm ci && npm cache clean --force

ADD . /frontend_app

RUN npm run build

ENV HOST 0.0.0.0
EXPOSE 3000

ENTRYPOINT ["node", ".output/server/index.mjs"]