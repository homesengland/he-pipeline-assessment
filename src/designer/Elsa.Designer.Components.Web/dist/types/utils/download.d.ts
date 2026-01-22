export interface DownloadOptions {
  fileName?: string;
  contentType?: string;
}
export declare function downloadFromUrl(url: string, options: DownloadOptions): void;
export declare function downloadFromBytes(content: Uint8Array, options: DownloadOptions): void;
export declare function downloadFromText(content: string, options: DownloadOptions): void;
export declare function downloadFromBlob(content: Blob, options: DownloadOptions): void;
