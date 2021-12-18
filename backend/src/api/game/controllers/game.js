"use strict";

/**
 *  game controller
 */

const { createCoreController } = require("@strapi/strapi").factories;

module.exports = createCoreController("api::game.game", ({ strapi }) => ({
  async find(ctx) {
    //only devs can create games
    const user = ctx.state.user;

    let games = await strapi.entityService.findMany("api::game.game", {
      filters: { owner: user.id },
      populate: {
        locations: true,
        players: true,
      },
    });

    return games;
  },
  async create(ctx) {
    //only devs can create games
    const user = ctx.state.user;

    //generate game code
    const { customAlphabet } = require("nanoid");
    const alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    const nanoid = customAlphabet(alphabet, 8);
    let gameCode = nanoid(); //=> "VANIKKLDJBUWQOQPSTPVG"

    //create game
    let newGame = await strapi.entityService.create("api::game.game", {
      data: {
        owner: user.id,
        gameCode: gameCode,
        locationsAmount: ctx.request.body.locationsAmount,
        locations: [],
        started: false,
      },
    });

    //create locations
    for (let i = 0; i < ctx.request.body.locationsAmount; i++) {
      await strapi.entityService.create("api::location.location", {
        data: {
          index: i,
          isBuildingUnit: false,
          game: newGame,
        },
      });
    }

    return newGame;
  },
  async join(ctx) {
    //only devs can create games
    const user = ctx.state.user;
    const id = ctx.params.id;

    //fetch game
    let game = await strapi.entityService.findMany("api::game.game", {
      filters: { gameCode: id },
      populate: {
        players: true,
      },
    });

    game = game[0];

    if (game == null)
      return ctx.badRequest(null, "Could not find game with the provided ID");

    //check if player is already in game
    let player = game.players.find((player) => player.user.id == user.id);
    if (player != null) return game;

    //update players
    await strapi.entityService.create("api::player.player", {
      data: {
        user: user.id,
        game: game.id,
        name: ctx.request.body.name,
        resources: game.startingResources,
        locationsOwned: [],
        unitsOwned: [],
      },
    });

    game = await strapi.entityService.findMany("api::game.game", {
      filters: { gameCode: id },
      populate: {
        players: true,
      },
    });

    game = game[0];

    return game;
  },
}));
