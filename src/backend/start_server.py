from datetime import datetime
from swagger_server.__main__ import main
from logger import logger

if __name__ == "__main__":
    logger.info("Starting the Swagger API server...")
    main()