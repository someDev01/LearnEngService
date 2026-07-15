import styles from '../tiles/tiles.module.css';

function Tiles({tiles, onTileClick}){
    return(
        <div className={styles.tiles}>
            {tiles.map((tile) => (
                <button
                    key={tile.id}
                    className={`${styles.tile} ${tile.isWrong ? styles.wrong : ''}`}
                    onClick={() => onTileClick(tile)}
                >
                    {tile.char.toUpperCase()}
                </button>
            ))}
        </div>
    )
}

export default Tiles;