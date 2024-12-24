import psycopg2
import requests
import os
import json
from datetime import datetime

if __name__ == '__main__':
    conn = psycopg2.connect(
        dbname=os.environ['DB_NAME'],
        user=os.environ['DB_USER'],
        password=os.environ['DB_PASS'],
        host=os.environ['DB_HOST'],
        port=os.environ['DB_PORT']
    )
    server_url = os.environ['SERVER_URL']
    
    cursor = conn.cursor()
    
    cursor.execute("SELECT id, body, headers, path FROM outbox_messages WHERE status = 'pending'")
    messages = cursor.fetchall()
    
    for message in messages:
        message_id, body, headers, path = message
        try:
            response = requests.post(f'${server_url}/${path}',
                                     json=json.loads(body),
                                     headers=json.loads(headers))
            if response.status_code == 200:
                cursor.execute("UPDATE outbox_messages SET status = 'sent', updated_at = %s WHERE id = %s",
                               (datetime.now(), message_id))
            else:
                cursor.execute("UPDATE outbox_messages SET status = 'failed', updated_at = %s WHERE id = %s",
                               (datetime.now(), message_id))
        except Exception:
            cursor.execute("UPDATE outbox_messages SET status = 'failed', updated_at = %s WHERE id = %s",
                           (datetime.now(), message_id))
    
    conn.commit()
    
    cursor.close() 
    conn.close()
