export interface Sharding {
  name: string;
  databaseName: string | null;
  connectionName: string;
  databaseType: string;
}

export interface ShardingRequest {
  name: string;
  databaseName: string;
  connectionName: string;
  databaseType: string;
}


export interface DbDetailsResponse {
    databaseInfo: any;
    allPossibleConnectionNames: string[];
    possibleDatabaseTypes: string[];
}