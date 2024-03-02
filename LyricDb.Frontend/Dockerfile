FROM node:20 AS base
ENV NODE_ENV=production
EXPOSE 3000

FROM base as install
WORKDIR /src
COPY . .
RUN npm install

# Publish
FROM install as publish
WORKDIR /src
RUN npm run build

# Final
FROM base as final
WORKDIR /app
COPY --from=publish /src/.output .
ENTRYPOINT ["node", "server/index.mjs"]