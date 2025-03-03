FROM node:lts AS base
WORKDIR /app

#Setup Env vars
ENV NODE_ENV=production

# By copying only the package.json and package-lock.json here, we ensure that the following `-deps` steps are independent of the source code.
# Therefore, the `-deps` steps will be skipped if only the source code changes.
COPY package.json .

FROM base AS prod-deps
RUN npm install --omit=dev

FROM base AS build-deps
# install build deps with development references, because some projects just need it for some reason
# -> And we want to be safe
RUN npm install

FROM build-deps AS build
COPY . .
RUN npm run build

FROM base AS final
COPY --from=prod-deps /app/node_modules ./node_modules
COPY --from=build /app/dist ./dist

# Webfrontend Port Configured in astro.config.mjs
EXPOSE 4999

ENTRYPOINT ["npm", "run", "start"]
