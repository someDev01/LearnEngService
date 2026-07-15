import styles from '../slots/slots.module.css';

 function Slots({target, wordProgressIndex, builded}){
    return(
        <div className={styles.slots}>
            {target.split('').map((char, i) => {               
                const filled = i < wordProgressIndex;
                return(
                    <div key={i} className={`${styles.slot} ${filled ? builded ? styles.builded : styles.filled : ''}`}>
                        {filled ? target[i].toUpperCase() : ''}
                    </div>
                )
            })}
        </div>
    )
 }

 export default Slots;