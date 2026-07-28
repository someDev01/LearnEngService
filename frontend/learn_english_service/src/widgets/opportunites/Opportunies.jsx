import { useState } from 'react';
import styles from '../opportunites/opportunies.module.css';
import Opportunity from '../../ui/oppotrunity/Opportunity';

function Opportunites(){

    const options = [
        {
            title: 'Короткие видео',
            content: 'Изучай английский с помощью коротких YouTube-видео, выбирая контент по своему уровню и интересам.',
            type: 'clip',
        },
        {
            title: 'Словарь с заметками',
            content: 'Добавляй незнакомые слова в личный словарь, чтобы легко повторять и закреплять новую лексику.',
            type: 'dict',
        },
        {
            title: 'Тренировка на слух',
            content: 'Слушайте произношение слов и собирайте их по буквам, чтобы лучше запоминать написание и понимать речь на слух.',
            type: 'train',
        }
    ];

    return(
        <div className={styles.opportunites}>
            <header className={styles.how_work_info}>
                <p>Возможности сервиса</p>
            </header>
            <div className={styles.cards}>
                {options.map((op, index) => (
                    <Opportunity
                        key={index}
                        index={index}
                        title={op.title}
                        type={op.type}
                        description={op.content}
                    />
                ))}
            </div>
        </div>
    )
}

export default Opportunites;