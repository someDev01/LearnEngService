import styles from '../card_content/card_content.module.css';

function CardContent({item, onMouseDown}){
    
    return(
        <div className={styles.card} onMouseDown={onMouseDown}>
            <div className={styles.img_card}>
                <img src={item.poster} alt=''/>
            </div>
            <div className={styles.card_name}>
                <p>{item.title}</p>
            </div>
        </div>
    )
}

export default CardContent;