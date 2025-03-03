// @ts-check
import {defineConfig} from 'astro/config';

import node from '@astrojs/node';

// https://astro.build/config
export default defineConfig({
    adapter: node({
        mode: 'standalone'
    }),
    server: {
        port: 4999,
        host: "0.0.0.0"
    }
});
