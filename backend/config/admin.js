module.exports = ({ env }) => ({
  auth: {
    secret: env('ADMIN_JWT_SECRET', 'ca01832a40cf9bfc5c0d8b92844bb027'),
  },
});
