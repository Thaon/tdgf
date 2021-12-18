"use strict";

/**
 * game router.
 */

const { createCoreRouter } = require("@strapi/strapi").factories;

module.exports = {
  routes: [
    {
      method: "GET",
      path: "/games",
      handler: "game.find",
    },
    {
      method: "POST",
      path: "/games",
      handler: "game.create",
    },
    {
      method: "POST",
      path: "/games/join/:id",
      handler: "game.join",
    },
  ],
};
