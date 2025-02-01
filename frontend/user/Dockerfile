FROM node:20 AS build
WORKDIR /app
COPY package.json yarn.lock ./
RUN yarn install
COPY . .
RUN yarn build

FROM node:20
WORKDIR /app
COPY --from=build /app ./
CMD ["yarn", "start"]