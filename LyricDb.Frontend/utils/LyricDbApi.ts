/* eslint-disable */
/* tslint:disable */
/*
 * ---------------------------------------------------------------
 * ## THIS FILE WAS GENERATED VIA SWAGGER-TYPESCRIPT-API        ##
 * ##                                                           ##
 * ## AUTHOR: acacode                                           ##
 * ## SOURCE: https://github.com/acacode/swagger-typescript-api ##
 * ---------------------------------------------------------------
 */

export interface ALRCFile {
  $schema?: string | null;
  li?: ALRCLyricInfo;
  si?: Record<string, string>;
  h?: ALRCHeader;
  l?: ALRCLine[] | null;
}

export interface ALRCHeader {
  s?: ALRCStyle[] | null;
}

export interface ALRCLine {
  id?: string | null;
  p?: string | null;
  /** @format int64 */
  f?: number | null;
  /** @format int64 */
  t?: number | null;
  s?: string | null;
  comment?: string | null;
  tx?: string | null;
  lt?: string | null;
  tr?: string | null;
  w?: ALRCWord[] | null;
}

export interface ALRCLyricInfo {
  lng?: string | null;
  author?: string | null;
  translation?: string | null;
  timeline?: string | null;
  transliteration?: string | null;
  proofread?: string | null;
  /** @format int32 */
  offset?: number | null;
  /** @format int64 */
  duration?: number | null;
}

export interface ALRCStyle {
  id?: string | null;
  p?: ALRCStylePosition;
  c?: string | null;
  t?: ALRCStyleAccent;
  h?: boolean;
}

/** @format int32 */
export enum ALRCStyleAccent {
  Value0 = 0,
  Value1 = 1,
  Value2 = 2,
  Value3 = 3,
}

/** @format int32 */
export enum ALRCStylePosition {
  Value0 = 0,
  Value1 = 1,
  Value2 = 2,
  Value3 = 3,
}

export interface ALRCWord {
  /** @format int64 */
  f?: number;
  /** @format int64 */
  t?: number;
  w?: string | null;
  s?: string | null;
  l?: string | null;
}

export interface HttpValidationProblemDetails {
  type?: string | null;
  title?: string | null;
  /** @format int32 */
  status?: number | null;
  detail?: string | null;
  instance?: string | null;
  errors?: Record<string, string[]>;
  [key: string]: any;
}

export interface LyricCreateRequest {
  /** @format uuid */
  songId?: string;
  content?: string | null;
  author?: string | null;
  translator?: string | null;
  transliterator?: string | null;
  timeline?: string | null;
  proofreader?: string | null;
}

export interface LyricInfoResponse {
  /** @format uuid */
  id?: string;
  /** @format date-time */
  createTime?: string;
  submitter?: UserInfoResponse;
  song?: SongInfoResponse;
  reviewer?: UserInfoResponse;
  status?: LyricStatus;
  author?: string | null;
  translator?: string | null;
  transliterator?: string | null;
  timeline?: string | null;
  proofreader?: string | null;
}

export interface LyricInfoResponsePagedResponseBase {
  /** @format int32 */
  page?: number;
  /** @format int32 */
  pageSize?: number;
  /** @format int32 */
  totalCount?: number;
  /** @format int32 */
  totalPages?: number;
  hasPrevious?: boolean;
  hasNext?: boolean;
  previousPage?: string | null;
  nextPage?: string | null;
  items?: LyricInfoResponse[] | null;
}

export interface LyricPutRequest {
  /** @format uuid */
  id?: string;
  /** @format uuid */
  songId?: string;
  content?: string | null;
  /** @format int32 */
  status?: number;
  author?: string | null;
  translator?: string | null;
  transliterator?: string | null;
  timeline?: string | null;
  proofreader?: string | null;
}

/** @format int32 */
export enum LyricStatus {
  Value0 = 0,
  Value1 = 1,
  Value2 = 2,
}

export interface ProblemDetails {
  type?: string | null;
  title?: string | null;
  /** @format int32 */
  status?: number | null;
  detail?: string | null;
  instance?: string | null;
  [key: string]: any;
}

export interface SongInfoResponse {
  /** @format uuid */
  id?: string;
  name?: string | null;
  artists?: string | null;
  album?: string | null;
  submitter?: UserInfoResponse;
  lyrics?: string[] | null;
  /** @format uuid */
  currentLyric?: string;
  cover?: string | null;
  /** @format date-time */
  createTime?: string;
  binds?: string[] | null;
}

export interface SongInfoResponsePagedResponseBase {
  /** @format int32 */
  page?: number;
  /** @format int32 */
  pageSize?: number;
  /** @format int32 */
  totalCount?: number;
  /** @format int32 */
  totalPages?: number;
  hasPrevious?: boolean;
  hasNext?: boolean;
  previousPage?: string | null;
  nextPage?: string | null;
  items?: SongInfoResponse[] | null;
}

export interface SongPostRequest {
  name?: string | null;
  artists?: string | null;
  album?: string | null;
  /** @format int32 */
  duration?: number;
  cover?: string | null;
  binds?: string[] | null;
}

export interface SongPutRequest {
  name?: string | null;
  artists?: string | null;
  album?: string | null;
  /** @format int32 */
  duration?: number;
  cover?: string | null;
  binds?: string[] | null;
  /** @format uuid */
  currentLyric?: string;
}

export interface UserInfoResponse {
  /** @format uuid */
  id?: string;
  userName?: string | null;
  avatar?: string | null;
  role?: UserRole;
  /** @format int32 */
  contributionPoint?: number;
}

export interface UserLoginRequest {
  account?: string | null;
  password?: string | null;
}

export interface UserPutRequest {
  /** @format uuid */
  id?: string;
  userName?: string | null;
  oldPassword?: string | null;
  password?: string | null;
  email?: string | null;
  role?: UserRole;
}

export interface UserRegisterRequest {
  name?: string | null;
  password?: string | null;
  email?: string | null;
}

/** @format int32 */
export enum UserRole {
  Value0 = 0,
  Value1 = 1,
  Value2 = 2,
  Value3 = 3,
}

import type { AxiosInstance, AxiosRequestConfig, AxiosResponse, HeadersDefaults, ResponseType } from "axios";
import axios from "axios";

export type QueryParamsType = Record<string | number, any>;

export interface FullRequestParams extends Omit<AxiosRequestConfig, "data" | "params" | "url" | "responseType"> {
  /** set parameter to `true` for call `securityWorker` for this request */
  secure?: boolean;
  /** request path */
  path: string;
  /** content type of request body */
  type?: ContentType;
  /** query params */
  query?: QueryParamsType;
  /** format of response (i.e. response.json() -> format: "json") */
  format?: ResponseType;
  /** request body */
  body?: unknown;
}

export type RequestParams = Omit<FullRequestParams, "body" | "method" | "query" | "path">;

export interface ApiConfig<SecurityDataType = unknown> extends Omit<AxiosRequestConfig, "data" | "cancelToken"> {
  securityWorker?: (
    securityData: SecurityDataType | null,
  ) => Promise<AxiosRequestConfig | void> | AxiosRequestConfig | void;
  secure?: boolean;
  format?: ResponseType;
}

export enum ContentType {
  Json = "application/json",
  FormData = "multipart/form-data",
  UrlEncoded = "application/x-www-form-urlencoded",
  Text = "text/plain",
}

export class HttpClient<SecurityDataType = unknown> {
  public instance: AxiosInstance;
  private securityData: SecurityDataType | null = null;
  private securityWorker?: ApiConfig<SecurityDataType>["securityWorker"];
  private secure?: boolean;
  private format?: ResponseType;

  constructor({ securityWorker, secure, format, ...axiosConfig }: ApiConfig<SecurityDataType> = {}) {
    this.instance = axios.create({ ...axiosConfig, baseURL: axiosConfig.baseURL || "" });
    this.secure = secure;
    this.format = format;
    this.securityWorker = securityWorker;
  }

  public setSecurityData = (data: SecurityDataType | null) => {
    this.securityData = data;
  };

  protected mergeRequestParams(params1: AxiosRequestConfig, params2?: AxiosRequestConfig): AxiosRequestConfig {
    const method = params1.method || (params2 && params2.method);

    return {
      ...this.instance.defaults,
      ...params1,
      ...(params2 || {}),
      headers: {
        ...((method && this.instance.defaults.headers[method.toLowerCase() as keyof HeadersDefaults]) || {}),
        ...(params1.headers || {}),
        ...((params2 && params2.headers) || {}),
      },
    };
  }

  protected stringifyFormItem(formItem: unknown) {
    if (typeof formItem === "object" && formItem !== null) {
      return JSON.stringify(formItem);
    } else {
      return `${formItem}`;
    }
  }

  protected createFormData(input: Record<string, unknown>): FormData {
    return Object.keys(input || {}).reduce((formData, key) => {
      const property = input[key];
      const propertyContent: any[] = property instanceof Array ? property : [property];

      for (const formItem of propertyContent) {
        const isFileType = formItem instanceof Blob || formItem instanceof File;
        formData.append(key, isFileType ? formItem : this.stringifyFormItem(formItem));
      }

      return formData;
    }, new FormData());
  }

  public request = async <T = any, _E = any>({
    secure,
    path,
    type,
    query,
    format,
    body,
    ...params
  }: FullRequestParams): Promise<AxiosResponse<T>> => {
    const secureParams =
      ((typeof secure === "boolean" ? secure : this.secure) &&
        this.securityWorker &&
        (await this.securityWorker(this.securityData))) ||
      {};
    const requestParams = this.mergeRequestParams(params, secureParams);
    const responseFormat = format || this.format || undefined;

    if (type === ContentType.FormData && body && body !== null && typeof body === "object") {
      body = this.createFormData(body as Record<string, unknown>);
    }

    if (type === ContentType.Text && body && body !== null && typeof body !== "string") {
      body = JSON.stringify(body);
    }

    return this.instance.request({
      ...requestParams,
      headers: {
        ...(requestParams.headers || {}),
        ...(type && type !== ContentType.FormData ? { "Content-Type": type } : {}),
      },
      params: query,
      responseType: responseFormat,
      data: body,
      url: path,
    });
  };
}

/**
 * @title LyricDb.Web
 * @version 1.0
 */
export class Api<SecurityDataType extends unknown> extends HttpClient<SecurityDataType> {
  lyric = {
    /**
     * No description
     *
     * @tags LyricEndpoint
     * @name GetPagedLyrics
     * @request GET:/lyric
     */
    getPagedLyrics: (
      query?: {
        /**
         * @format int32
         * @default 0
         */
        page?: number;
        /**
         * @format int32
         * @default 10
         */
        pageSize?: number;
        /**
         * @format int32
         * @default -1
         */
        status?: number;
      },
      params: RequestParams = {},
    ) =>
      this.request<LyricInfoResponsePagedResponseBase, any>({
        path: `/lyric`,
        method: "GET",
        query: query,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags LyricEndpoint
     * @name PostLyric
     * @request POST:/lyric
     */
    postLyric: (data: LyricCreateRequest, params: RequestParams = {}) =>
      this.request<LyricInfoResponse, ProblemDetails | void>({
        path: `/lyric`,
        method: "POST",
        body: data,
        type: ContentType.Json,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags LyricEndpoint
     * @name PutLyric
     * @request PUT:/lyric
     */
    putLyric: (data: LyricPutRequest, params: RequestParams = {}) =>
      this.request<LyricInfoResponse, ProblemDetails | void>({
        path: `/lyric`,
        method: "PUT",
        body: data,
        type: ContentType.Json,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags LyricEndpoint
     * @name GetLyric
     * @request GET:/lyric/{id}
     */
    getLyric: (id: string, params: RequestParams = {}) =>
      this.request<LyricInfoResponse, void>({
        path: `/lyric/${id}`,
        method: "GET",
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags LyricEndpoint
     * @name GetLyricByType
     * @request GET:/lyric/{id}/{type}
     */
    getLyricByType: (id: string, type: string, params: RequestParams = {}) =>
      this.request<LyricInfoResponse, void>({
        path: `/lyric/${id}/${type}`,
        method: "GET",
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags LyricEndpoint
     * @name GetLyricContentByType
     * @request GET:/lyric/{id}/{type}/contents
     */
    getLyricContentByType: (id: string, type: string, params: RequestParams = {}) =>
      this.request<string, void>({
        path: `/lyric/${id}/${type}/contents`,
        method: "GET",
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags LyricEndpoint
     * @name GetLyricContent
     * @request GET:/lyric/{id}/contents
     */
    getLyricContent: (id: string, params: RequestParams = {}) =>
      this.request<ALRCFile, void>({
        path: `/lyric/${id}/contents`,
        method: "GET",
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags LyricEndpoint
     * @name PostLyricType
     * @request POST:/lyric/{type}
     */
    postLyricType: (type: string, data: LyricCreateRequest, params: RequestParams = {}) =>
      this.request<LyricInfoResponse, void | ProblemDetails>({
        path: `/lyric/${type}`,
        method: "POST",
        body: data,
        type: ContentType.Json,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags LyricEndpoint
     * @name PutLyricType
     * @request PUT:/lyric/{type}
     */
    putLyricType: (type: string, data: LyricPutRequest, params: RequestParams = {}) =>
      this.request<LyricInfoResponse, void | ProblemDetails>({
        path: `/lyric/${type}`,
        method: "PUT",
        body: data,
        type: ContentType.Json,
        format: "json",
        ...params,
      }),
  };
  song = {
    /**
     * No description
     *
     * @tags SongEndpoint
     * @name GetPagedSongs
     * @request GET:/song
     */
    getPagedSongs: (
      query?: {
        /**
         * @format int32
         * @default 0
         */
        page?: number;
        /**
         * @format int32
         * @default 10
         */
        pageSize?: number;
        /** @default "" */
        search?: string;
        /** @default false */
        unlyriced?: boolean;
      },
      params: RequestParams = {},
    ) =>
      this.request<SongInfoResponsePagedResponseBase, any>({
        path: `/song`,
        method: "GET",
        query: query,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags SongEndpoint
     * @name PostSong
     * @request POST:/song
     */
    postSong: (data: SongPostRequest, params: RequestParams = {}) =>
      this.request<SongInfoResponse, ProblemDetails>({
        path: `/song`,
        method: "POST",
        body: data,
        type: ContentType.Json,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags SongEndpoint
     * @name GetSong
     * @request GET:/song/{id}
     */
    getSong: (id: string, params: RequestParams = {}) =>
      this.request<SongInfoResponse, void>({
        path: `/song/${id}`,
        method: "GET",
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags SongEndpoint
     * @name PutSong
     * @request PUT:/song/{id}
     */
    putSong: (id: string, data: SongPutRequest, params: RequestParams = {}) =>
      this.request<SongInfoResponse, ProblemDetails>({
        path: `/song/${id}`,
        method: "PUT",
        body: data,
        type: ContentType.Json,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags SongEndpoint
     * @name DeleteSongById
     * @request DELETE:/song/{id}
     */
    deleteSongById: (id: string, params: RequestParams = {}) =>
      this.request<SongInfoResponse, ProblemDetails | void>({
        path: `/song/${id}`,
        method: "DELETE",
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags SongEndpoint
     * @name GetSongLyrics
     * @request GET:/song/{id}/lyric
     */
    getSongLyrics: (id: string, params: RequestParams = {}) =>
      this.request<LyricInfoResponse[], void>({
        path: `/song/${id}/lyric`,
        method: "GET",
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags SongEndpoint
     * @name SetSongLyric
     * @request PUT:/song/{id}/lyric
     */
    setSongLyric: (
      id: string,
      query: {
        /** @format uuid */
        lyricId: string;
      },
      params: RequestParams = {},
    ) =>
      this.request<void, void>({
        path: `/song/${id}/lyric`,
        method: "PUT",
        query: query,
        ...params,
      }),

    /**
     * No description
     *
     * @tags SongEndpoint
     * @name GetSongByBind
     * @request GET:/song/bind/{bind}
     */
    getSongByBind: (bind: string, params: RequestParams = {}) =>
      this.request<SongInfoResponse, void>({
        path: `/song/bind/${bind}`,
        method: "GET",
        format: "json",
        ...params,
      }),
  };
  user = {
    /**
     * No description
     *
     * @tags UserEndpoint
     * @name PostRegister
     * @request POST:/user/register
     */
    postRegister: (data: UserRegisterRequest, params: RequestParams = {}) =>
      this.request<void, HttpValidationProblemDetails>({
        path: `/user/register`,
        method: "POST",
        body: data,
        type: ContentType.Json,
        ...params,
      }),

    /**
     * No description
     *
     * @tags UserEndpoint
     * @name PostLogin
     * @request POST:/user/login
     */
    postLogin: (data: UserLoginRequest, params: RequestParams = {}) =>
      this.request<UserInfoResponse, ProblemDetails>({
        path: `/user/login`,
        method: "POST",
        body: data,
        type: ContentType.Json,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags UserEndpoint
     * @name GetUserPagedLyrics
     * @request POST:/user/{userId}/lyrics
     */
    getUserPagedLyrics: (
      userId: string,
      query: {
        /** @format uuid */
        userId: string;
        /**
         * @format int32
         * @default 0
         */
        page?: number;
        /**
         * @format int32
         * @default 10
         */
        pageSize?: number;
      },
      params: RequestParams = {},
    ) =>
      this.request<LyricInfoResponsePagedResponseBase, ProblemDetails>({
        path: `/user/${userId}/lyrics`,
        method: "POST",
        query: query,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags UserEndpoint
     * @name GetUserInfo
     * @request GET:/user/{userId}
     */
    getUserInfo: (userId: string, params: RequestParams = {}) =>
      this.request<UserInfoResponse, ProblemDetails | void>({
        path: `/user/${userId}`,
        method: "GET",
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags UserEndpoint
     * @name GetMyUserInfo
     * @request GET:/user
     */
    getMyUserInfo: (params: RequestParams = {}) =>
      this.request<UserInfoResponse, ProblemDetails>({
        path: `/user`,
        method: "GET",
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags UserEndpoint
     * @name UpdateUserInfo
     * @request PUT:/user
     */
    updateUserInfo: (data: UserPutRequest, params: RequestParams = {}) =>
      this.request<UserInfoResponse, void | HttpValidationProblemDetails | ProblemDetails>({
        path: `/user`,
        method: "PUT",
        body: data,
        type: ContentType.Json,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags UserEndpoint
     * @name ConfirmEmail
     * @request GET:/user/{id}/verify/email
     */
    confirmEmail: (
      id: string,
      query: {
        token: string;
      },
      params: RequestParams = {},
    ) =>
      this.request<void, void>({
        path: `/user/${id}/verify/email`,
        method: "GET",
        query: query,
        ...params,
      }),
  };
}
