using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;

public class OracleDbContext : IDisposable
{
    private readonly OracleConnection _conn;

    public OracleDbContext(string connectionString)
    {
        _conn = new OracleConnection(connectionString);
        try
        {
            _conn.Open();
        }
        catch (OracleException ex)
        {
            throw new Exception($"Oracle 연결 실패 (코드:{ex.Number}): {ex.Message}", ex);
        }
        catch (Exception ex)
        {
            throw new Exception($"Oracle 연결 중 알 수 없는 오류: {ex.Message}", ex);
        }
    }

    public List<T> ExecuteList<T>(string query, Dictionary<string, object> parameters, Func<OracleDataReader, T> mapFunc)
    {
        var result = new List<T>();
        try
        {
            using (var cmd = _conn.CreateCommand())
            {
                cmd.CommandText = query;

                if (parameters != null)
                {
                    foreach (var param in parameters)
                        cmd.Parameters.Add(new OracleParameter(param.Key, param.Value));
                }

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        result.Add(mapFunc(reader));
                }
            }
        }
        catch (OracleException ex)
        {
            throw new Exception($"오라클 쿼리 실행 오류 (코드:{ex.Number}): {ex.Message}", ex);
        }
        catch (Exception ex)
        {
            throw new Exception($"처리 중 오류 발생: {ex.Message}", ex);
        }
        return result;
    }

    public void Dispose()
    {
        if (_conn != null)
        {
            if (_conn.State != System.Data.ConnectionState.Closed)
                _conn.Close();
            _conn.Dispose();
        }
    }
}
