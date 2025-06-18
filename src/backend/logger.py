import os
import logging
from datetime import datetime

log_dir = os.path.join(os.path.dirname(__file__), "logs")
os.makedirs(log_dir, exist_ok=True)

today = datetime.now().strftime("%Y-%m-%d")
log_path = os.path.join(log_dir, f"app_{today}.log")

logging.basicConfig(
    level=logging.INFO,
    format='%(asctime)s [%(levelname)s] %(message)s',
    handlers=[
        logging.FileHandler(log_path, encoding="utf-8"),
        logging.StreamHandler()
    ]
)

logger = logging.getLogger("LaendlefinderLogger")