/// <reference types="vite/client" />

interface ImportMetaEnv {
    readonly VITE_API_URL: string;
    readonly VITE_PAGE_SIZE: number;
}

interface ImportMeta {
    readonly env: ImportMetaEnv
}